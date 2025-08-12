/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RoomCategoryDto2 } from '../models/RoomCategoryDto2';
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class RoomCategoryService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @returns any OK
     * @throws ApiError
     */
    public getRoomCategory(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/RoomCategory',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public postRoomCategory(
        requestBody: RoomCategoryDto2,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/RoomCategory',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public getRoomCategory1(
        id: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/RoomCategory/{id}',
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
    public deleteRoomCategory(
        id: number,
    ): CancelablePromise<boolean> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/RoomCategory/{id}',
            path: {
                'id': id,
            },
        });
    }
}
