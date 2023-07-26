/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { Tenant } from './Tenant';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{{environmentId}}/tenants');

export interface TenantsForEnvironmentArguments {
    environmentId: string;
}
export class TenantsForEnvironment extends ObservableQueryFor<Tenant[], TenantsForEnvironmentArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{{environmentId}}/tenants';
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

    static use(args?: TenantsForEnvironmentArguments): [QueryResultWithState<Tenant[]>] {
        return useObservableQuery<Tenant[], TenantsForEnvironment, TenantsForEnvironmentArguments>(TenantsForEnvironment, args);
    }
}
