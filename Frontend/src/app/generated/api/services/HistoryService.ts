/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { HistoryDto } from '../models/HistoryDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class HistoryService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @returns any OK
     * @throws ApiError
     */
    public getHistory(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/History',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public postHistory(
        requestBody: HistoryDto,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/History',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public getHistory1(
        id: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/History/{id}',
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
    public deleteHistory(
        id: number,
    ): CancelablePromise<boolean> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/History/{id}',
            path: {
                'id': id,
            },
        });
    }
}
