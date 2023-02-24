/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { SecretsForMicroservice } from './SecretsForMicroservice';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/secrets');

export interface SecretsForMicroserviceIdArguments {
    applicationId: string;
    environmentId: string;
    microserviceId: string;
}
export class SecretsForMicroserviceId extends ObservableQueryFor<SecretsForMicroservice, SecretsForMicroserviceIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/secrets';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: SecretsForMicroservice = {} as any;

    constructor() {
        super(SecretsForMicroservice, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
        ];
    }

    static use(args?: SecretsForMicroserviceIdArguments): [QueryResultWithState<SecretsForMicroservice>] {
        return useObservableQuery<SecretsForMicroservice, SecretsForMicroserviceId, SecretsForMicroserviceIdArguments>(SecretsForMicroserviceId, args);
    }
}
