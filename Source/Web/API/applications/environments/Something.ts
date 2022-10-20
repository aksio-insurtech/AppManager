/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { ApplicationEnvironment } from './ApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments');

export interface SomethingArguments {
    applicationId: string;
}
export class Something extends QueryFor<ApplicationEnvironment[], SomethingArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationEnvironment[] = [];

    constructor() {
        super(ApplicationEnvironment, true);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    static use(args?: SomethingArguments): [QueryResultWithState<ApplicationEnvironment[]>, PerformQuery<SomethingArguments>] {
        return useQuery<ApplicationEnvironment[], Something, SomethingArguments>(Something, args);
    }
}
