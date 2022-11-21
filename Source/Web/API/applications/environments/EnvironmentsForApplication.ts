/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { ApplicationEnvironment } from './ApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/environments-for-application');

export interface EnvironmentsForApplicationArguments {
    applicationId: string;
}
export class EnvironmentsForApplication extends ObservableQueryFor<ApplicationEnvironment[], EnvironmentsForApplicationArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/environments-for-application';
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

    static use(args?: EnvironmentsForApplicationArguments): [QueryResultWithState<ApplicationEnvironment[]>] {
        return useObservableQuery<ApplicationEnvironment[], EnvironmentsForApplication, EnvironmentsForApplicationArguments>(EnvironmentsForApplication, args);
    }
}
