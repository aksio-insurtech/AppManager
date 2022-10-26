/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { EnvironmentVariablesForDeployable } from './EnvironmentVariablesForDeployable';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables/{{deployableId}}/environment-variables');

export interface EnvironmentVariablesForMicroserviceIdArguments {
    deployableId: string;
}
export class EnvironmentVariablesForMicroserviceId extends ObservableQueryFor<EnvironmentVariablesForDeployable, EnvironmentVariablesForMicroserviceIdArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables/{{deployableId}}/environment-variables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: EnvironmentVariablesForDeployable = {} as any;

    constructor() {
        super(EnvironmentVariablesForDeployable, false);
    }

    get requestArguments(): string[] {
        return [
            'deployableId',
        ];
    }

    static use(args?: EnvironmentVariablesForMicroserviceIdArguments): [QueryResultWithState<EnvironmentVariablesForDeployable>] {
        return useObservableQuery<EnvironmentVariablesForDeployable, EnvironmentVariablesForMicroserviceId, EnvironmentVariablesForMicroserviceIdArguments>(EnvironmentVariablesForMicroserviceId, args);
    }
}
