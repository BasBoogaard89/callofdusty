/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { TextTemplateDto } from '../models/TextTemplateDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class TextTemplateService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @returns any OK
     * @throws ApiError
     */
    public getTextTemplate(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/TextTemplate',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public postTextTemplate(
        requestBody: TextTemplateDto,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/TextTemplate',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public getTextTemplate1(
        id: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/TextTemplate/{id}',
            path: {
                'id': id,
            },
        });
    }
    /**
     * @param id
     * @returns boolean OK
     * @throws ApiError
     */
    public deleteTextTemplate(
        id: number,
    ): CancelablePromise<boolean> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/TextTemplate/{id}',
            path: {
                'id': id,
            },
        });
    }
}
