/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ChoreCategoryDto2 } from './ChoreCategoryDto2';
import type { DirtinessFactor } from './DirtinessFactor';
import type { RoomDto } from './RoomDto';
export type ChoreDto = {
    description: string;
    categoryId: number;
    roomId: number;
    durationMinutes: number;
    frequencyDays: number;
    dirtinessFactor: DirtinessFactor;
    room?: RoomDto;
    category?: ChoreCategoryDto2;
    id?: number;
};

