/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { ConfigFilesForApplicationEnvironment } from './ConfigFilesForApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{{environmentId}}/config-files');

export interface ConfigFilesForApplicationEnvironmentIdArguments {
    environmentId: string;
}
export class ConfigFilesForApplicationEnvironmentId extends ObservableQueryFor<ConfigFilesForApplicationEnvironment, ConfigFilesForApplicationEnvironmentIdArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{{environmentId}}/config-files';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ConfigFilesForApplicationEnvironment = {} as any;

    constructor() {
        super(ConfigFilesForApplicationEnvironment, false);
    }

    get requestArguments(): string[] {
        return [
            'environmentId',
        ];
    }

    static use(args?: ConfigFilesForApplicationEnvironmentIdArguments): [QueryResultWithState<ConfigFilesForApplicationEnvironment>] {
        return useObservableQuery<ConfigFilesForApplicationEnvironment, ConfigFilesForApplicationEnvironmentId, ConfigFilesForApplicationEnvironmentIdArguments>(ConfigFilesForApplicationEnvironmentId, args);
    }
}
