import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginRequest } from '@generated/api';
import { AuthAdapterService } from '@infrastructure/adapters/auth-adapter.service';
import { BasePageController } from '@infrastructure/controllers/base-page.controller';
import { MATERIAL_IMPORTS } from '@infrastructure/material.imports';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        ...MATERIAL_IMPORTS
    ],
    templateUrl: './login.html'
})
export class Login extends BasePageController {
    authService = inject(AuthAdapterService);

    form = new FormGroup({
        email: new FormControl('', Validators.required),
        password: new FormControl('', Validators.required),
    });

    ngOnInit() {
        this.setDynamicTitle();
    }

    submit() {
        const request = this.form.value as LoginRequest;

        this.authService.login(request)
            .then(() => {
                this.router.navigateByUrl('/');
            })
            .catch(err => {
                console.error('Login failed', err);
            });
    }
}
