/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { EnvironmentVariablesForDeployable } from './EnvironmentVariablesForDeployable';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/environment-variables');

export interface EnvironmentVariablesForDeployableIdArguments {
    applicationId: string;
    environmentId: string;
    microserviceId: string;
    deployableId: string;
}
export class EnvironmentVariablesForDeployableId extends ObservableQueryFor<EnvironmentVariablesForDeployable, EnvironmentVariablesForDeployableIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables/{{deployableId}}/environment-variables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: EnvironmentVariablesForDeployable = {} as any;

    constructor() {
        super(EnvironmentVariablesForDeployable, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
            'deployableId',
        ];
    }

    static use(args?: EnvironmentVariablesForDeployableIdArguments): [QueryResultWithState<EnvironmentVariablesForDeployable>] {
        return useObservableQuery<EnvironmentVariablesForDeployable, EnvironmentVariablesForDeployableId, EnvironmentVariablesForDeployableIdArguments>(EnvironmentVariablesForDeployableId, args);
    }
}
