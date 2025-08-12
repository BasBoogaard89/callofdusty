import { Signal } from "@angular/core";

export interface BaseListConfig<T> {
    items: Signal<T[]>;
    columns: string[];
    onAdd?: () => void;
    onEdit?: (item: T) => void;
    onDelete?: (id: number) => void;

    disableAdd?: boolean;
    disableEdit?: boolean;
    disableDelete?: boolean;

    customSort?: Record<string, (value: any, row: T) => any>;
}
