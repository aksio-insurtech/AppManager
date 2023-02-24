/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { ConfigFilesForDeployable } from './ConfigFilesForDeployable';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/config-files');

export interface ConfigFilesForDeployableIdArguments {
    applicationId: string;
    environmentId: string;
    microserviceId: string;
    deployableId: string;
}
export class ConfigFilesForDeployableId extends ObservableQueryFor<ConfigFilesForDeployable, ConfigFilesForDeployableIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/config-files';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ConfigFilesForDeployable = {} as any;

    constructor() {
        super(ConfigFilesForDeployable, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'deployableId',
        ];
    }

    static use(args?: ConfigFilesForDeployableIdArguments): [QueryResultWithState<ConfigFilesForDeployable>] {
        return useObservableQuery<ConfigFilesForDeployable, ConfigFilesForDeployableId, ConfigFilesForDeployableIdArguments>(ConfigFilesForDeployableId, args);
    }
}
