/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { ConfigFilesForMicroservice } from './ConfigFilesForMicroservice';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/config-files');

export interface ConfigFilesForMicroserviceIdArguments {
    applicationId: string;
    environmentId: string;
    microserviceId: string;
}
export class ConfigFilesForMicroserviceId extends ObservableQueryFor<ConfigFilesForMicroservice, ConfigFilesForMicroserviceIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/config-files';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: ConfigFilesForMicroservice = {} as any;

    constructor() {
        super(ConfigFilesForMicroservice, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
        ];
    }

    static use(args?: ConfigFilesForMicroserviceIdArguments): [QueryResultWithState<ConfigFilesForMicroservice>] {
        return useObservableQuery<ConfigFilesForMicroservice, ConfigFilesForMicroserviceId, ConfigFilesForMicroserviceIdArguments>(ConfigFilesForMicroserviceId, args);
    }
}
