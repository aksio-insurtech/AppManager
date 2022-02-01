/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResult, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { Organization } from './Organization';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organizations/current');

export class CurrentOrganization extends QueryFor<Organization> {
    readonly route: string = '/api/organizations/current';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Organization = {} as any;
    readonly requiresArguments: boolean = false;

    static use(): [QueryResult<Organization>, PerformQuery] {
        return useQuery<Organization, CurrentOrganization>(CurrentOrganization);
    }
}
