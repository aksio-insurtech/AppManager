/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { CustomDomainsForApplicationEnvironment } from './CustomDomainsForApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/custom-domains/http');

export interface BlahArguments {
    applicationId: string;
    environmentId: string;
}
export class Blah extends QueryFor<CustomDomainsForApplicationEnvironment, BlahArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/custom-domains/http';
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

    static use(args?: BlahArguments): [QueryResultWithState<CustomDomainsForApplicationEnvironment>, PerformQuery<BlahArguments>] {
        return useQuery<CustomDomainsForApplicationEnvironment, Blah, BlahArguments>(Blah, args);
    }
}
