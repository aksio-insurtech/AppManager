/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { EnvironmentVariablesForMicroservice } from './EnvironmentVariablesForMicroservice';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/environment-variables');

export interface EnvironmentVariablesForMicroserviceIdArguments {
    applicationId: string;
    environmentId: string;
    microserviceId: string;
}
export class EnvironmentVariablesForMicroserviceId extends ObservableQueryFor<EnvironmentVariablesForMicroservice, EnvironmentVariablesForMicroserviceIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/environment-variables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: EnvironmentVariablesForMicroservice = {} as any;

    constructor() {
        super(EnvironmentVariablesForMicroservice, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
        ];
    }

    static use(args?: EnvironmentVariablesForMicroserviceIdArguments): [QueryResultWithState<EnvironmentVariablesForMicroservice>] {
        return useObservableQuery<EnvironmentVariablesForMicroservice, EnvironmentVariablesForMicroserviceId, EnvironmentVariablesForMicroserviceIdArguments>(EnvironmentVariablesForMicroserviceId, args);
    }
}
