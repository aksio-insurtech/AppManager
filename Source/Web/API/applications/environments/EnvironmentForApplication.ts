/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { ApplicationEnvironment } from './ApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}');

export interface EnvironmentForApplicationArguments {
    applicationId: string;
    environmentId: string;
}
export class EnvironmentForApplication extends QueryFor<ApplicationEnvironment, EnvironmentForApplicationArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ApplicationEnvironment = {} as any;

    constructor() {
        super(ApplicationEnvironment, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    static use(args?: EnvironmentForApplicationArguments): [QueryResultWithState<ApplicationEnvironment>, PerformQuery<EnvironmentForApplicationArguments>] {
        return useQuery<ApplicationEnvironment, EnvironmentForApplication, EnvironmentForApplicationArguments>(EnvironmentForApplication, args);
    }
}
