/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { ApplicationHierarchyForListing } from './ApplicationHierarchyForListing';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/hierarchy');

export class ApplicationsHierarchy extends ObservableQueryFor<ApplicationHierarchyForListing[]> {
    readonly route: string = '/api/applications/hierarchy';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationHierarchyForListing[] = [];

    constructor() {
        super(ApplicationHierarchyForListing, true);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    static use(): [QueryResultWithState<ApplicationHierarchyForListing[]>] {
        return useObservableQuery<ApplicationHierarchyForListing[], ApplicationsHierarchy>(ApplicationsHierarchy);
    }
}
