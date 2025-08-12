/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ChoreDto } from '../models/ChoreDto';
import type { ChoreDto2 } from '../models/ChoreDto2';
import type { ChoreFilterDto } from '../models/ChoreFilterDto';
import type { ChoreQuestDto } from '../models/ChoreQuestDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';
export class ChoreService {
    constructor(public readonly httpRequest: BaseHttpRequest) {}
    /**
     * @param requestBody
     * @returns ChoreDto OK
     * @throws ApiError
     */
    public postChoreGetAllFiltered(
        requestBody: ChoreFilterDto,
    ): CancelablePromise<Array<ChoreDto>> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/Chore/GetAllFiltered',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param themeId
     * @returns ChoreQuestDto OK
     * @throws ApiError
     */
    public getChoreQuest(
        themeId?: number,
    ): CancelablePromise<ChoreQuestDto> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Chore/Quest',
            query: {
                'themeId': themeId,
            },
        });
    }
    /**
     * @param themeId
     * @returns ChoreQuestDto OK
     * @throws ApiError
     */
    public getChoreAllQuests(
        themeId?: number,
    ): CancelablePromise<Array<ChoreQuestDto>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Chore/AllQuests',
            query: {
                'themeId': themeId,
            },
        });
    }
    /**
     * @returns any OK
     * @throws ApiError
     */
    public getChore(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Chore',
        });
    }
    /**
     * @param requestBody
     * @returns any OK
     * @throws ApiError
     */
    public postChore(
        requestBody: ChoreDto2,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/Chore',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param id
     * @returns any OK
     * @throws ApiError
     */
    public getChore1(
        id: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Chore/{id}',
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
    public deleteChore(
        id: number,
    ): CancelablePromise<boolean> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/Chore/{id}',
            path: {
                'id': id,
            },
        });
    }
}
