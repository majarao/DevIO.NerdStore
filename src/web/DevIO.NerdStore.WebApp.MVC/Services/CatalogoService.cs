﻿using DevIO.NerdStore.WebApp.MVC.Extensions;
using DevIO.NerdStore.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public class CatalogoService : Service, ICatalogoService
{
    private HttpClient HttpClient { get; }

    public CatalogoService(HttpClient httpClient, IOptions<AppSettings> appSetting)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(appSetting.Value.CatalogoUrl);
    }

    public async Task<ProdutoViewModel?> ObterPorId(Guid id)
    {
        HttpResponseMessage response = await HttpClient.GetAsync($"/catalogo/produtos/{id}");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<ProdutoViewModel>(response);
    }

    public async Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string? query = null)
    {
        HttpResponseMessage response = await HttpClient.GetAsync($"/catalogo/produtos?ps={pageSize}&page={pageIndex}&q={query}");

        TratarErrosResponse(response);

        return await DeserializarObjetoResponse<PagedViewModel<ProdutoViewModel>>(response) ?? new();
    }
}
