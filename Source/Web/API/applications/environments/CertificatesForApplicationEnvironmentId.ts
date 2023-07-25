/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { CertificatesForApplicationEnvironment } from './CertificatesForApplicationEnvironment';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{{environmentId}}/certificates');

export interface CertificatesForApplicationEnvironmentIdArguments {
    environmentId: string;
}
export class CertificatesForApplicationEnvironmentId extends ObservableQueryFor<CertificatesForApplicationEnvironment, CertificatesForApplicationEnvironmentIdArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{{environmentId}}/certificates';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: CertificatesForApplicationEnvironment = {} as any;

    constructor() {
        super(CertificatesForApplicationEnvironment, false);
    }

    get requestArguments(): string[] {
        return [
            'environmentId',
        ];
    }

    static use(args?: CertificatesForApplicationEnvironmentIdArguments): [QueryResultWithState<CertificatesForApplicationEnvironment>] {
        return useObservableQuery<CertificatesForApplicationEnvironment, CertificatesForApplicationEnvironmentId, CertificatesForApplicationEnvironmentIdArguments>(CertificatesForApplicationEnvironmentId, args);
    }
}
