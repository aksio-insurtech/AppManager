/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResult, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { Organization } from './Organization';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organizations');

export class AllOrganizations extends ObservableQueryFor<Organization[]> {
    readonly route: string = '/api/organizations';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Organization[] = [];

    get requestArguments(): string[] {
        return [
        ];
    }

    static use(): [QueryResult<Organization[]>] {
        return useObservableQuery<Organization[], AllOrganizations>(AllOrganizations);
    }
}
