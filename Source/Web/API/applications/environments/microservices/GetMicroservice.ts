/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { Microservice } from './Microservice';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}');

export interface GetMicroserviceArguments {
    applicationId: string;
    environmentId: string;
    microserviceId: string;
}
export class GetMicroservice extends QueryFor<Microservice, GetMicroserviceArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Microservice = {} as any;

    constructor() {
        super(Microservice, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
        ];
    }

    static use(args?: GetMicroserviceArguments): [QueryResultWithState<Microservice>, PerformQuery<GetMicroserviceArguments>] {
        return useQuery<Microservice, GetMicroservice, GetMicroserviceArguments>(GetMicroservice, args);
    }
}
