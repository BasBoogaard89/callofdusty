import { Component, computed, effect, input, ViewChild } from '@angular/core';
import { MAT_BUTTON_CONFIG } from '@angular/material/button';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';
import { MATERIAL_IMPORTS } from '@infrastructure/material.imports';
import { HumanizeColumnPipe } from '@infrastructure/pipes/humanize-column.pipe';
import { ValueByPathPipe } from '@infrastructure/pipes/value-by-path.pipe';

@Component({
    selector: 'app-base-list',
    standalone: true,
    imports: [
        ValueByPathPipe,
        HumanizeColumnPipe,
        ...MATERIAL_IMPORTS
    ],
    providers: [
        {
            provide: MAT_BUTTON_CONFIG,
            useValue: {
                color: 'primary',
                defaultAppearance: 'filled'
            }
        }
    ],
    templateUrl: './base-list.html'
})
export class BaseList<T extends { id?: number }> {
    config = input.required<BaseListConfig<T>>();
    @ViewChild(MatSort) sort: MatSort;

    dataSource: MatTableDataSource<T> = new MatTableDataSource();
    displayedColumns = computed(() => [...this.config().columns, 'actions']);

    _dataEffect = effect(() => {
        this.dataSource.data = this.config().items();
    });

    ngAfterViewInit() {
        this.dataSource.sort = this.sort;
        this.dataSource.sortingDataAccessor = (item, property) => {
            if (this.config().customSort?.[property]) {
                return this.config().customSort[property](item[property], item);
            }

            return property.split('.').reduce((obj, key) => obj?.[key], item);
        }
    }
}
