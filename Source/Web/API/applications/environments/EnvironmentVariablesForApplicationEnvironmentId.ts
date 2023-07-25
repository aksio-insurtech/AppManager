/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { EnvironmentVariablesForApplicationEnvironment } from './EnvironmentVariablesForApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{{environmentId}}/environment-variables');

export interface EnvironmentVariablesForApplicationEnvironmentIdArguments {
    environmentId: string;
}
export class EnvironmentVariablesForApplicationEnvironmentId extends ObservableQueryFor<EnvironmentVariablesForApplicationEnvironment, EnvironmentVariablesForApplicationEnvironmentIdArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{{environmentId}}/environment-variables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: EnvironmentVariablesForApplicationEnvironment = {} as any;

    constructor() {
        super(EnvironmentVariablesForApplicationEnvironment, false);
    }

    get requestArguments(): string[] {
        return [
            'environmentId',
        ];
    }

    static use(args?: EnvironmentVariablesForApplicationEnvironmentIdArguments): [QueryResultWithState<EnvironmentVariablesForApplicationEnvironment>] {
        return useObservableQuery<EnvironmentVariablesForApplicationEnvironment, EnvironmentVariablesForApplicationEnvironmentId, EnvironmentVariablesForApplicationEnvironmentIdArguments>(EnvironmentVariablesForApplicationEnvironmentId, args);
    }
}
