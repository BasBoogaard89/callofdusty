/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RoomDto3 } from '../models/RoomDto3';
import type { RoomFilterDto } from '../models/RoomFilterDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class RoomService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @param requestBody
     * @returns RoomDto3 OK
     * @throws ApiError
     */
    public postRoomGetAllFiltered(
        requestBody: RoomFilterDto,
    ): CancelablePromise<Array<RoomDto3>> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/Room/GetAllFiltered',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public getRoom(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Room',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public postRoom(
        requestBody: RoomDto3,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/Room',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public getRoom1(
        id: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Room/{id}',
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
    public deleteRoom(
        id: number,
    ): CancelablePromise<boolean> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/Room/{id}',
            path: {
                'id': id,
            },
        });
    }
}
