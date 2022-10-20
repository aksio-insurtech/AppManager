/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { Tenant } from './Tenant';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{{environmentId}}/tenants/blah');

export interface BlahArguments {
    environmentId: string;
}
export class Blah extends QueryFor<Tenant[], BlahArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{{environmentId}}/tenants/blah';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Tenant[] = [];

    constructor() {
        super(Tenant, true);
    }

    get requestArguments(): string[] {
        return [
            'environmentId',
        ];
    }

    static use(args?: BlahArguments): [QueryResultWithState<Tenant[]>, PerformQuery<BlahArguments>] {
        return useQuery<Tenant[], Blah, BlahArguments>(Blah, args);
    }
}
