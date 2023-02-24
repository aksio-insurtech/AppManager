/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { SecretsForApplicationEnvironment } from './SecretsForApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{{environmentId}}/secrets');

export interface SecretsForApplicationEnvironmentIdArguments {
    environmentId: string;
}
export class SecretsForApplicationEnvironmentId extends ObservableQueryFor<SecretsForApplicationEnvironment, SecretsForApplicationEnvironmentIdArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{{environmentId}}/secrets';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: SecretsForApplicationEnvironment = {} as any;

    constructor() {
        super(SecretsForApplicationEnvironment, false);
    }

    get requestArguments(): string[] {
        return [
            'environmentId',
        ];
    }

    static use(args?: SecretsForApplicationEnvironmentIdArguments): [QueryResultWithState<SecretsForApplicationEnvironment>] {
        return useObservableQuery<SecretsForApplicationEnvironment, SecretsForApplicationEnvironmentId, SecretsForApplicationEnvironmentIdArguments>(SecretsForApplicationEnvironmentId, args);
    }
}
