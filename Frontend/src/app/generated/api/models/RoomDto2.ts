/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ChoreDto } from './ChoreDto';
import type { RoomCategoryDto } from './RoomCategoryDto';
export type RoomDto2 = {
    description: string;
    categoryId: number;
    chores?: Array<ChoreDto>;
    category?: RoomCategoryDto;
    id?: number;
};

