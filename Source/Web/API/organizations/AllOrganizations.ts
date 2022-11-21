/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { Organization } from './Organization';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organizations');

export class AllOrganizations extends ObservableQueryFor<Organization[]> {
    readonly route: string = '/api/organizations';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Organization[] = [];

    constructor() {
        super(Organization, true);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    static use(): [QueryResultWithState<Organization[]>] {
        return useObservableQuery<Organization[], AllOrganizations>(AllOrganizations);
    }
}
