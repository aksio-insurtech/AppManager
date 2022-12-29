/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { SecretsForApplication } from './SecretsForApplication';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/secrets');

export interface SecretsForApplicationIdArguments {
    applicationId: string;
}
export class SecretsForApplicationId extends ObservableQueryFor<SecretsForApplication, SecretsForApplicationIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/secrets';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: SecretsForApplication = {} as any;

    constructor() {
        super(SecretsForApplication, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    static use(args?: SecretsForApplicationIdArguments): [QueryResultWithState<SecretsForApplication>] {
        return useObservableQuery<SecretsForApplication, SecretsForApplicationId, SecretsForApplicationIdArguments>(SecretsForApplicationId, args);
    }
}
