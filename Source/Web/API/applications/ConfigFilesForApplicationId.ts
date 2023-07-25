/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { ConfigFilesForApplication } from './ConfigFilesForApplication';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/config-files');

export interface ConfigFilesForApplicationIdArguments {
    applicationId: string;
}
export class ConfigFilesForApplicationId extends ObservableQueryFor<ConfigFilesForApplication, ConfigFilesForApplicationIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/config-files';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ConfigFilesForApplication = {} as any;

    constructor() {
        super(ConfigFilesForApplication, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    static use(args?: ConfigFilesForApplicationIdArguments): [QueryResultWithState<ConfigFilesForApplication>] {
        return useObservableQuery<ConfigFilesForApplication, ConfigFilesForApplicationId, ConfigFilesForApplicationIdArguments>(ConfigFilesForApplicationId, args);
    }
}
