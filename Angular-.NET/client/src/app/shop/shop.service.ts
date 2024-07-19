import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = 'https://localhost:5001/api/'
  constructor(private Http: HttpClient) { }

  getProducts(brandId?: number, typeId?: number ) {
    let params = new HttpParams();

    if (brandId) {
      params = params.append('brandId', brandId);
    }

    if (typeId) {
      params = params.append('typeId', typeId);
    }

    return this.Http.get<Pagination<Product[]>>(this.baseUrl + 'products', {params});
  }

  getBrands() {
    return this.Http.get<Brand[]>(this.baseUrl + 'products/brands');
  }

  getTypes() {
    return this.Http.get<Type[]>(this.baseUrl + 'products/types');
  }
}   
