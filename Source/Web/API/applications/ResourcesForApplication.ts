/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { ApplicationResources } from './ApplicationResources';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/resources/{{applicationId}}');

export interface ResourcesForApplicationArguments {
    applicationId: string;
}
export class ResourcesForApplication extends QueryFor<ApplicationResources, ResourcesForApplicationArguments> {
    readonly route: string = '/api/applications/resources/{{applicationId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationResources = {} as any;

    constructor() {
        super(ApplicationResources, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    static use(args?: ResourcesForApplicationArguments): [QueryResultWithState<ApplicationResources>, PerformQuery<ResourcesForApplicationArguments>] {
        return useQuery<ApplicationResources, ResourcesForApplication, ResourcesForApplicationArguments>(ResourcesForApplication, args);
    }
}
