/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { EnvironmentVariablesForApplication } from './EnvironmentVariablesForApplication';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environment-variables');

export interface EnvironmentVariablesForApplicationIdArguments {
    applicationId: string;
}
export class EnvironmentVariablesForApplicationId extends ObservableQueryFor<EnvironmentVariablesForApplication, EnvironmentVariablesForApplicationIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environment-variables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: EnvironmentVariablesForApplication = {} as any;

    constructor() {
        super(EnvironmentVariablesForApplication, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    static use(args?: EnvironmentVariablesForApplicationIdArguments): [QueryResultWithState<EnvironmentVariablesForApplication>] {
        return useObservableQuery<EnvironmentVariablesForApplication, EnvironmentVariablesForApplicationId, EnvironmentVariablesForApplicationIdArguments>(EnvironmentVariablesForApplicationId, args);
    }
}
