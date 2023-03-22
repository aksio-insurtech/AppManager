/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/cratis-applications-frontend/queries';
import { AppSettingsForMicroservice } from './AppSettingsForMicroservice';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/app-settings');

export interface AppSettingsForMicroserviceIdArguments {
    applicationId: string;
    environmentId: string;
    microserviceId: string;
}
export class AppSettingsForMicroserviceId extends ObservableQueryFor<AppSettingsForMicroservice, AppSettingsForMicroserviceIdArguments> {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/microservices/{{microserviceId}}/app-settings';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: AppSettingsForMicroservice = {} as any;

    constructor() {
        super(AppSettingsForMicroservice, false);
    }

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
            'microserviceId',
        ];
    }

    static use(args?: AppSettingsForMicroserviceIdArguments): [QueryResultWithState<AppSettingsForMicroservice>] {
        return useObservableQuery<AppSettingsForMicroservice, AppSettingsForMicroserviceId, AppSettingsForMicroserviceIdArguments>(AppSettingsForMicroserviceId, args);
    }
}
