/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { ObservableQueryFor, QueryResultWithState, useObservableQuery } from '@aksio/applications/queries';
import { Ingress } from './Ingress';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{environmentId}/ingresses/{{ingressId}}');

export interface IngressByIdArguments {
    ingressId: string;
}
export class IngressById extends ObservableQueryFor<Ingress, IngressByIdArguments> {
    readonly route: string = '/api/applications/{applicationId}/environments/{environmentId}/ingresses/{{ingressId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly defaultValue: Ingress = {} as any;

    constructor() {
        super(Ingress, false);
    }

    get requestArguments(): string[] {
        return [
            'ingressId',
        ];
    }

    static use(args?: IngressByIdArguments): [QueryResultWithState<Ingress>] {
        return useObservableQuery<Ingress, IngressById, IngressByIdArguments>(IngressById, args);
    }
}
