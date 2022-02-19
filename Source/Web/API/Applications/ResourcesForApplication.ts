/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResult, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
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
    readonly requiresArguments: boolean = true;

    static use(args?: ResourcesForApplicationArguments): [QueryResult<ApplicationResources>, PerformQuery<ResourcesForApplicationArguments>] {
        return useQuery<ApplicationResources, ResourcesForApplication, ResourcesForApplicationArguments>(ResourcesForApplication, args);
    }
}
