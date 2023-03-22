/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { AppSettingsForApplicationEnvironment } from './AppSettingsForApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{{environmentId}}/app-settings');

export interface AppSettingsForApplicationEnvironmentIdArguments {
    environmentId: string;
}
export class AppSettingsForApplicationEnvironmentId extends ObservableQueryFor<AppSettingsForApplicationEnvironment, AppSettingsForApplicationEnvironmentIdArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{{environmentId}}/app-settings';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: AppSettingsForApplicationEnvironment = {} as any;

    constructor() {
        super(AppSettingsForApplicationEnvironment, false);
    }

    get requestArguments(): string[] {
        return [
            'environmentId',
        ];
    }

    static use(args?: AppSettingsForApplicationEnvironmentIdArguments): [QueryResultWithState<AppSettingsForApplicationEnvironment>] {
        return useObservableQuery<AppSettingsForApplicationEnvironment, AppSettingsForApplicationEnvironmentId, AppSettingsForApplicationEnvironmentIdArguments>(AppSettingsForApplicationEnvironmentId, args);
    }
}
