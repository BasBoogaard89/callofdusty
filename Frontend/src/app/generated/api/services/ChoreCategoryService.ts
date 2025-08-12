/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ChoreCategoryDto } from '../models/ChoreCategoryDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class ChoreCategoryService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @returns any OK
     * @throws ApiError
     */
    public getChoreCategory(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/ChoreCategory',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public postChoreCategory(
        requestBody: ChoreCategoryDto,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/ChoreCategory',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public getChoreCategory1(
        id: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/ChoreCategory/{id}',
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
    public deleteChoreCategory(
        id: number,
    ): CancelablePromise<boolean> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/ChoreCategory/{id}',
            path: {
                'id': id,
            },
        });
    }
}
