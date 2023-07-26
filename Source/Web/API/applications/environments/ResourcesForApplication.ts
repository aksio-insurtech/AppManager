/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/applications/queries';
import { ApplicationEnvironmentResources } from './ApplicationEnvironmentResources';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{environmentId}/resources');

export interface ResourcesForApplicationArguments {
    applicationId: string;
}
export class ResourcesForApplication extends QueryFor<ApplicationEnvironmentResources, ResourcesForApplicationArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{environmentId}/resources';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationEnvironmentResources = {} as any;

    constructor() {
        super(ApplicationEnvironmentResources, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    static use(args?: ResourcesForApplicationArguments): [QueryResultWithState<ApplicationEnvironmentResources>, PerformQuery<ResourcesForApplicationArguments>] {
        return useQuery<ApplicationEnvironmentResources, ResourcesForApplication, ResourcesForApplicationArguments>(ResourcesForApplication, args);
    }
}
