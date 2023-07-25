/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/applications/queries';
import { CloudLocation } from './CloudLocation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/cloudlocations');

export class AllCloudLocations extends QueryFor<CloudLocation[]> {
    readonly route: string = '/api/cloudlocations';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: CloudLocation[] = [];

    constructor() {
        super(CloudLocation, true);
    }

    get requestArguments(): string[] {
        return [
        ];
    }

    static use(): [QueryResultWithState<CloudLocation[]>, PerformQuery] {
        return useQuery<CloudLocation[], AllCloudLocations>(AllCloudLocations);
    }
}
