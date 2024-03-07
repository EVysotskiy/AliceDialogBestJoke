import requests

from abc import ABC, abstractmethod
from bs4 import BeautifulSoup

class BaseParser(ABC):
    SOUP_PARSER = 'html.parser'
    PAGES_ENCODING = 'utf-8'

    def __init__(self):
        self.headers = {}

    @abstractmethod
    def _parse_soup(soup):
        pass

    def _get_content(self, url, custom_headers = None):
        headers = custom_headers if custom_headers is not None else self.headers
        response = requests.get(url, headers=headers)
        response.encoding = self.PAGES_ENCODING

        return response.text

    def _parse_html(self, content):
        soup = BeautifulSoup(content, self.SOUP_PARSER)
        output = self._parse_soup(soup)
        
        return output

    def _get_items_from_url(self, url):
        print(url)
        content = self._get_content(url)
        products = self._parse_html(content)
        return products
        
    def _generate_items(self, url):
        yield self._get_items_from_url(url)

    def get_catalog(self):
        catalog = []
        for url in self.links:
            for products in self._generate_items(url):
                catalog.extend(products)
        
        return catalog
