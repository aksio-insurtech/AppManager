/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResult, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { ApplicationsHierarchyForListing } from './ApplicationsHierarchyForListing';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications');

export class AllApplications extends ObservableQueryFor<ApplicationsHierarchyForListing[]> {
    readonly route: string = '/api/applications';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationsHierarchyForListing[] = [];
    readonly requiresArguments: boolean = false;

    static use(): [QueryResult<ApplicationsHierarchyForListing[]>] {
        return useObservableQuery<ApplicationsHierarchyForListing[], AllApplications>(AllApplications);
    }
}
