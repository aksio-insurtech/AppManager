/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { CustomDomainsForApplicationEnvironment } from './CustomDomainsForApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/custom-domains');

export interface CustomDomainConfigurationForApplicationEnvironmentArguments {
    applicationId: string;
    environmentId: string;
}
export class CustomDomainConfigurationForApplicationEnvironment extends ObservableQueryFor<CustomDomainsForApplicationEnvironment, CustomDomainConfigurationForApplicationEnvironmentArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/custom-domains';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: CustomDomainsForApplicationEnvironment = {} as any;

    constructor() {
        super(CustomDomainsForApplicationEnvironment, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    static use(args?: CustomDomainConfigurationForApplicationEnvironmentArguments): [QueryResultWithState<CustomDomainsForApplicationEnvironment>] {
        return useObservableQuery<CustomDomainsForApplicationEnvironment, CustomDomainConfigurationForApplicationEnvironment, CustomDomainConfigurationForApplicationEnvironmentArguments>(CustomDomainConfigurationForApplicationEnvironment, args);
    }
}
