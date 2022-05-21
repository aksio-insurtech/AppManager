/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResult, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { Microservice } from './Microservice';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/microservices/{{microserviceId}}');

export interface GetMicroserviceArguments {
    microserviceId: string;
}
export class GetMicroservice extends QueryFor<Microservice, GetMicroserviceArguments> {
    readonly route: string = '/api/applications/{applicationId}/microservices/{{microserviceId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Microservice = {} as any;

    get requestArguments(): string[] {
        return [
            'microserviceId',
        ];
    }

    static use(args?: GetMicroserviceArguments): [QueryResult<Microservice>, PerformQuery<GetMicroserviceArguments>] {
        return useQuery<Microservice, GetMicroservice, GetMicroserviceArguments>(GetMicroservice, args);
    }
}
