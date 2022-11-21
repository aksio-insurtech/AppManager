/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { QueryFor, QueryResultWithState, useQuery, PerformQuery } from '@aksio/cratis-applications-frontend/queries';
import { Microservice } from './Microservice';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices');

export interface MicroservicesInEnvironmentArguments {
    applicationId: string;
    environmentId: string;
}
export class MicroservicesInEnvironment extends QueryFor<Microservice[], MicroservicesInEnvironmentArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Microservice[] = [];

    constructor() {
        super(Microservice, true);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    static use(args?: MicroservicesInEnvironmentArguments): [QueryResultWithState<Microservice[]>, PerformQuery<MicroservicesInEnvironmentArguments>] {
        return useQuery<Microservice[], MicroservicesInEnvironment, MicroservicesInEnvironmentArguments>(MicroservicesInEnvironment, args);
    }
}
