/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResult, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { CloudLocation } from './CloudLocation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/cloudlocations');

export class AllCloudLocations extends QueryFor<CloudLocation[]> {
    readonly route: string = '/api/cloudlocations';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: CloudLocation[] = [];

    get requestArguments(): string[] {
        return [
        ];
    }

    static use(): [QueryResult<CloudLocation[]>, PerformQuery] {
        return useQuery<CloudLocation[], AllCloudLocations>(AllCloudLocations);
    }
}
