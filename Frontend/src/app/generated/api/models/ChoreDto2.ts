/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ChoreCategoryDto2 } from './ChoreCategoryDto2';
import type { DirtinessFactor } from './DirtinessFactor';
import type { RoomDto2 } from './RoomDto2';
export type ChoreDto2 = {
    description: string;
    categoryId: number;
    roomId: number;
    durationMinutes: number;
    frequencyDays: number;
    dirtinessFactor: DirtinessFactor;
    room?: RoomDto2;
    category?: ChoreCategoryDto2;
    id?: number;
};

