import { Component, Input } from '@angular/core';
import { FilterDescriptor, CompositeFilterDescriptor } from '@progress/kendo-data-query';
import { FilterService } from '@progress/kendo-angular-grid';

const flatten = filter => {
    const filters = (filter || {}).filters;
    if (filters) {
        return filters.reduce((acc, curr) => acc.concat(curr.filters ? flatten(curr) : [curr]), []);
    }
    return [];
};

@Component({
    selector: 'my-filter-wrapper',
    templateUrl: './multiselect.component.html',
})


export class MultiSelectWrapperComponent {
    @Input() public data: any[];
    @Input() public filter: any;
    @Input() public filterService: any;

    private categoryFilter: any[] = [];

    public categoryFilters(filter: CompositeFilterDescriptor): FilterDescriptor[] {
        this.categoryFilter.splice(
            0, this.categoryFilter.length,
            ...flatten(filter).map(({ value }) => value)

        );
        return this.categoryFilter;
    }

    public categoryChange(values: any[], filterService: FilterService): void {

        filterService.filter({
            filters: values.map(value => ({
                field: 'DaySpeaking',
                operator: 'eq',
                value
            })),
            logic: 'or'
        });

    }
}
