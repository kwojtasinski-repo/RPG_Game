<template>
    <div class="searchBar" v-if="filterBy">
        <div class="input-group mb-2">
            <input type="search" class="form-control" v-model='searchQuery' :placeholder="`Search by ${filterBy}`" @change="searchValueChanged">
        </div>
    </div>
    <table class="table table-hover table-striped table-fit table-bordered">
        <thead>
            <tr class="table-dark">
                <th v-for="field, i in fields" :key='i' @click="sortTable(i)" > 
                    {{field}}
                    <svg v-if="sortable" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-sort-alpha-down" viewBox="0 0 16 16">
                        <path fill-rule="evenodd" d="M10.082 5.629 9.664 7H8.598l1.789-5.332h1.234L13.402 7h-1.12l-.419-1.371h-1.781zm1.57-.785L11 2.687h-.047l-.652 2.157h1.351z"/>
                        <path d="M12.96 14H9.028v-.691l2.579-3.72v-.054H9.098v-.867h3.785v.691l-2.567 3.72v.054h2.645V14zM4.5 2.5a.5.5 0 0 0-1 0v9.793l-1.146-1.147a.5.5 0 0 0-.708.708l2 1.999.007.007a.497.497 0 0 0 .7-.006l2-2a.5.5 0 0 0-.707-.708L4.5 12.293V2.5z"/>
                    </svg>
                </th>
            </tr>
        </thead>
        <tbody class="table-group-divider">
            <tr v-for="item in dataPerPage" :key='item' :class="markedElement && markedElement.id === item.id ? 'bg-info' : 'table-light'" @click="() => setCellAsMarked(item)">
                <td v-for="(value, field) in item" :key='field'>{{ value }}</td>
            </tr>
        </tbody>
    </table>
    <div class="d-flex background-pager-click">
        <div class="page-clicker">
            <span v-for="n in pages" :key="n" class="col ms-2 me-2 text-primary link" @click="() => getNextPage(n)">
                {{n}}
            </span>
        </div>
    </div>
</template>

<script>
export default {
    name: 'TableComponent',
    props: {
        data: {
            type: Array,
            required: true
        },
        fields: {
            type: Array,
            required: true
        },
        sortable: {
            type: Boolean
        },
        filterBy: {
            type: String
        },
        elementsPerPage: {
            type: Number,
            default: 25
        },
    },
    data() {
        return {
            sortedData: [],
            lastIndex: -1,
            sortClick: 0,
            searchQuery: '',
            currentPage: 1,
            pages: 1,
            dataPerPage: [],
            markedElement: null
        }
    },
    methods: {
        sortTable(index) {
            if (!this.sortable) {
                console.log(index);
                return;
            }

            if (this.data.length <= 0) {
                return;
            }

            if (this.lastIndex === index) {
                this.sortClick++;
            } else {
                this.sortClick = 0;
            }

            this.lastIndex = index;
            
            let sort = 'asc';
            if (this.sortClick % 2 === 1) {
                sort = 'desc';
            }

            const fieldToSort = Object.keys(this.sortedData[0])[index];
            this.sortedData.sort(this.dynamicSort(fieldToSort, sort));
        },
        dynamicSort(property, sort) {
            var sortOrder = 1;
            if (property[0] === "-") {
                sortOrder = -1;
                property = property.substr(1);
            }
            return function (a,b) {
                if(sort === 'asc') {
                    let result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
                    return result * sortOrder;
                } else {
                    let result = (b[property] < a[property]) ? -1 : (b[property] > a[property]) ? 1 : 0;
                    return result * sortOrder;
                }
            }
        },
        searchValueChanged() {
          this.sortedData = this.data.filter(i => {
            return i[this.filterBy].toLowerCase().indexOf(this.searchQuery.toLowerCase()) !== -1;
          });
          this.calculatePages();
          this.setDataOnPage(1);
        },
        calculatePages() {
          const pages = Math.ceil(this.sortedData.length/this.elementsPerPage);
          this.pages = pages;
        },
        setDataOnPage(page) {
          const startIndex = (page - 1) * this.elementsPerPage;
          const endIndex = startIndex + this.elementsPerPage;
          this.dataPerPage = this.sortedData.slice(startIndex, endIndex);
        },
        getNextPage(page) {
          this.setDataOnPage(page);
        },
        setCellAsMarked(item) {
            if (item && this.markedElement && item.id == this.markedElement.id) {
                this.markedElement = null;
                return;
            }

            this.markedElement = item;
            this.$emit("markedElement", this.markedElement);
        }
    },
    created() {
        this.sortedData = [...this.data];
        this.calculatePages();
        this.setDataOnPage(1);
    }
}
</script>

<style>
  .table-striped tbody tr:nth-of-type(2n+1) {
    background-color: rgba(0,0,0,.01);
  }
  
  .page-clicker {
    margin-bottom: 0.5rem;
    margin-top: 0.5rem;
    display: inline-flex;
    flex-wrap: wrap;
  }

  .background-pager-click {
    position: relative;
    transform: translate(0, -1rem);
    background-color: #212529;
  }

  .link {
    cursor: pointer;
  }
</style>