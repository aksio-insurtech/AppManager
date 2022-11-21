/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { Deployable } from './Deployable';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables');

export interface DeployablesForMicroserviceArguments {
    applicationId: string;
    environmentId: string;
    microserviceId: string;
}
export class DeployablesForMicroservice extends ObservableQueryFor<Deployable[], DeployablesForMicroserviceArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/deployables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Deployable[] = [];

    constructor() {
        super(Deployable, true);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
        ];
    }

    static use(args?: DeployablesForMicroserviceArguments): [QueryResultWithState<Deployable[]>] {
        return useObservableQuery<Deployable[], DeployablesForMicroservice, DeployablesForMicroserviceArguments>(DeployablesForMicroservice, args);
    }
}
