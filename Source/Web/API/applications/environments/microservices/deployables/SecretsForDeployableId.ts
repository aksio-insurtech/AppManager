/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { SecretsForDeployable } from './SecretsForDeployable';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables/{{deployableId}}/secrets');

export interface SecretsForDeployableIdArguments {
    deployableId: string;
}
export class SecretsForDeployableId extends ObservableQueryFor<SecretsForDeployable, SecretsForDeployableIdArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{environmentId}/microservices/{microserviceId}/deployables/{{deployableId}}/secrets';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: SecretsForDeployable = {} as any;

    constructor() {
        super(SecretsForDeployable, false);
    }

    get requestArguments(): string[] {
        return [
            'deployableId',
        ];
    }

    static use(args?: SecretsForDeployableIdArguments): [QueryResultWithState<SecretsForDeployable>] {
        return useObservableQuery<SecretsForDeployable, SecretsForDeployableId, SecretsForDeployableIdArguments>(SecretsForDeployableId, args);
    }
}
